/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   prompt_utils.c                                     :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/22 18:49:41 by caide-so          #+#    #+#             */
/*   Updated: 2025/05/22 19:10:11 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

char	*get_user(void)
{
	char	*user;

	user = getenv("USER");
	if (!user)
		return ("unkown");
	return (user);
}

char	*read_hostname_file(void)
{
	int		fd;
	char	buf[256];
	ssize_t	len;

	fd = open("/etc/hostname", O_RDONLY);
	if (fd < 0)
		return (ft_strdup("unkown"));
	len = read(fd, buf, sizeof(buf) - 1);
	close(fd);
	if (len <= 0)
		return (ft_strdup("unkown"));
	buf[len] = '\0';
	if (buf[len - 1] == '\n')
		buf[len - 1] = '\0';
	return (ft_strdup(buf));
}

char	*get_cwd_display(void)
{
	char	*cwd;
	char	*home;
	char	*display;

	cwd = getcwd(NULL, 0);
	if (!cwd)
		return (ft_strdup("[deleted]"));
	home = getenv("HOME");
	if (cwd && home && ft_strncmp(cwd, home, ft_strlen(home)) == 0)
	{
		display = replace_home(cwd, home);
		return (display);
	}
	return (cwd);
}

char	*replace_home(char *cwd, char *home)
{
	size_t	h_len;
	char	*src;
	char	*shortc;

	if (!cwd || !home)
		return (ft_strdup("~"));
	h_len = ft_strlen(home);
	if (cwd[h_len] == '/')
		src = cwd + h_len + 1;
	else
		src = cwd + h_len;
	shortc = build_shortc(src, cwd);
	free(cwd);
	return (shortc);
}

char	*build_shortc(char *src, char *cwd)
{
	size_t	tail;
	char	*shortc;

	tail = ft_strlen(src);
	if (tail > 0)
		shortc = (char *)malloc(2 + tail + 1);
	else
		shortc = (char *)malloc(1 + 1);
	if (!shortc)
	{
		free(cwd);
		return (NULL);
	}
	shortc[0] = '~';
	if (tail > 0)
	{
		shortc[1] = '/';
		ft_strlcpy(shortc + 2, src, tail + 1);
	}
	else
		shortc[1] = '\0';
	return (shortc);
}
