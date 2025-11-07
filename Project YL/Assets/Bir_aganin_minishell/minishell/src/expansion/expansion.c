/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   expansion.c                                        :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: marcudos <marcudos@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/17 16:14:44 by marcudos          #+#    #+#             */
/*   Updated: 2025/05/27 17:15:09 by marcudos         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

void	expander(char ***args, t_env *env, int *size_args, t_shell *sh)
{
	t_args	*expanded_envs;
	t_args	*head;
	char	**expanded;

	if (!(*args) || !(*args)[0])
		return ;
	expanded_envs = expand_env(*args, env, sh);
	head = expand_wild(expanded_envs);
	free_args_list(expanded_envs);
	expanded = list_to_args(head, size_args);
	free_args_temp(*args);
	(*args) = expanded;
	free_args_list(head);
}

char	**list_to_args(t_args *args, int *size_args)
{
	t_args	*temp;
	char	**res;
	int		size;
	int		i;

	temp = args;
	size = 0;
	while (temp)
	{
		size++;
		temp = temp->next;
	}
	*size_args = size;
	res = (char **) malloc(sizeof(char *) * (size + 1));
	if (!res)
		return (NULL);
	i = 0;
	while (args)
	{
		res[i] = ft_strdup(args->arg);
		i++;
		args = args->next;
	}
	res[i] = NULL;
	return (res);
}
