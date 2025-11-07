/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   env_init.c                                         :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/04/23 10:26:29 by caide-so          #+#    #+#             */
/*   Updated: 2025/06/17 02:09:50 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

t_env	*init_env(char **envp)
{
	t_env	*head;
	t_env	*tail;
	int		i;

	head = NULL;
	tail = NULL;
	i = 0;
	while (envp[i])
	{
		if (!env_process(envp[i], &head, &tail))
		{
			env_free_all(&head);
			return (NULL);
		}
		i++;
	}
	return (head);
}

int	env_process(char *entry, t_env **head, t_env **tail)
{
	char	*sep;
	t_env	*node;

	sep = ft_strchr(entry, '=');
	if (!sep)
		return (1);
	*sep = '\0';
	node = env_new(entry, sep + 1);
	*sep = '=';
	if (!node)
		return (0);
	env_append(head, tail, node);
	return (1);
}

t_env	*env_new(const char *key, const char *value)
{
	t_env	*node;

	node = (t_env *)malloc(sizeof(t_env));
	if (!node)
		exit_perror("malloc");
	node->key = ft_strdup(key);
	if (!node->key)
		exit_perror("ft_strdup");
	if (value)
		node->value = ft_strdup(value);
	else
		node->value = NULL;
	if (value && !node->value)
		exit_perror("ft_strdup");
	node->next = NULL;
	return (node);
}

int	env_append(t_env **head, t_env **tail, t_env *node)
{
	if (!*head)
		*head = node;
	else
		(*tail)->next = node;
	*tail = node;
	return (1);
}

void	env_export_only(t_env **env_head, char *name)
{
	t_env	*curr;
	t_env	*node;

	curr = *env_head;
	while (curr)
	{
		if (ft_strcmp(curr->key, name) == 0)
			return ;
		curr = curr->next;
	}
	node = env_new(name, NULL);
	if (!node)
		exit_perror("env_new");
	if (*env_head == NULL)
		*env_head = node;
	else
	{
		curr = *env_head;
		while (curr->next)
			curr = curr->next;
		curr->next = node;
	}
}
